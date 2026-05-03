using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

//
// Summary:
//     Provides a method to match query text using a fuzzy search algorithm.
public static class FuzzySearch
{
    private struct ScoreIndx
    {
        public int i;

        public int score;

        public int prev_mi;
    }

    private class FuzzyMatchData : IDisposable
    {
        public List<ScoreIndx>[] matches_indx;

        public bool[,] matchData;

        private FuzzyMatchData(int strN, int patternN)
        {
            matchData = new bool[strN, patternN];
            matches_indx = new List<ScoreIndx>[patternN];
            for (int i = 0; i < patternN; i++)
            {
                matches_indx[i] = new List<ScoreIndx>(8);
            }
        }

        public void Dispose()
        {
        }

        public static FuzzyMatchData Request(int strN, int patternN)
        {
            return new FuzzyMatchData(strN, patternN);
        }
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    internal struct ScopedProfiler : IDisposable
    {
        public ScopedProfiler(string name)
        {
        }

        public ScopedProfiler(string name, UnityEngine.Object targetObject)
        {
        }

        public void Dispose()
        {
        }
    }

    public static bool FuzzyMatch(string pattern, string origin, List<int> matches = null)
    {
        long outScore = 0L;
        return FuzzyMatch(pattern, origin, ref outScore, matches);
    }

    public static bool FuzzyMatch(string pattern, string origin, ref long outScore, List<int> matches = null)
    {
        string text;
        int length;
        int i;
        int num2;
        using (new ScopedProfiler("[FM] Init"))
        {
            outScore = -100000L;
            matches?.Clear();
            if (string.IsNullOrEmpty(origin))
            {
                return false;
            }

            if (string.IsNullOrEmpty(pattern))
            {
                return true;
            }

            text = origin.ToLowerInvariant();
            pattern = pattern.ToLowerInvariant();
            length = pattern.Length;
            i = 0;
            int num = text.Length - 1;
            char c = pattern[0];
            char c2 = pattern[pattern.Length - 1];
            for (; i < text.Length && c != text[i]; i++)
            {
            }

            while (num >= 0 && c2 != text[num])
            {
                num--;
            }

            num++;
            num2 = num - i;
            if (num2 < length)
            {
                return false;
            }

            int num3 = 0;
            int num4 = i;
            while (num3 < length && num4 < num)
            {
                if (pattern[num3] == text[num4])
                {
                    num3++;
                }

                num4++;
            }

            if (num3 < length)
            {
                return false;
            }
        }

        using (new ScopedProfiler("[FM] Body"))
        {
            using FuzzyMatchData fuzzyMatchData = FuzzyMatchData.Request(num2, length);
            int num5 = num2 - length + 1;
            int val = 0;
            using (new ScopedProfiler("[FM] Match loop"))
            {
                for (int j = 0; j < length; j++)
                {
                    int num6 = num2 + 1;
                    bool flag = true;
                    int k = Math.Max(j, val);
                    for (int num7 = num5 + j; k < num7; k++)
                    {
                        if (text[k] == '<')
                        {
                            for (; k < num7 && text[k] != '>'; k++)
                            {
                            }
                        }

                        int index = k + i;
                        bool flag2 = false;
                        if (pattern[j] == text[index])
                        {
                            flag2 = true;
                        }

                        if (k >= fuzzyMatchData.matchData.GetLength(0) || j >= fuzzyMatchData.matchData.GetLength(1))
                        {
                            return false;
                        }

                        fuzzyMatchData.matchData[k, j] = flag2;
                        if (flag2)
                        {
                            if (flag)
                            {
                                num6 = k;
                                flag = false;
                            }

                            fuzzyMatchData.matches_indx[j].Add(new ScoreIndx
                            {
                                i = k,
                                score = 1,
                                prev_mi = -1
                            });
                        }
                    }

                    if (flag)
                    {
                        return false;
                    }

                    val = num6;
                }
            }

            int num8 = num2 - (matches?.Count ?? 0);
            using (new ScopedProfiler("[FM] Best score 0"))
            {
                for (int l = 0; l < fuzzyMatchData.matches_indx[0].Count; l++)
                {
                    int i2 = fuzzyMatchData.matches_indx[0][l].i;
                    int num9 = i + i2;
                    int num10 = 100 + -1 * num8;
                    int num11 = -5 * num9;
                    if (num11 < -15)
                    {
                        num11 = -15;
                    }

                    num10 += num11;
                    if (num9 == 0)
                    {
                        num10 += 35;
                    }
                    else
                    {
                        char c3 = origin[num9];
                        char c4 = origin[num9 - 1];
                        if (char.IsUpper(c3) && !char.IsUpper(c4))
                        {
                            num10 += 30;
                        }
                        else if (c4 == '_' || c4 == ' ')
                        {
                            num10 += 30;
                        }
                    }

                    fuzzyMatchData.matches_indx[0][l] = new ScoreIndx
                    {
                        i = i2,
                        score = num10,
                        prev_mi = -1
                    };
                }
            }

            using (new ScopedProfiler("[FM] Best score 1..pattern_n"))
            {
                for (int m = 1; m < length; m++)
                {
                    for (int n = 0; n < fuzzyMatchData.matches_indx[m].Count; n++)
                    {
                        ScoreIndx value = fuzzyMatchData.matches_indx[m][n];
                        int num12 = i + fuzzyMatchData.matches_indx[m][n].i;
                        char c5 = origin[num12];
                        char c6 = origin[num12 - 1];
                        if (char.IsUpper(c5) && !char.IsUpper(c6))
                        {
                            value.score += 30;
                        }
                        else if (c6 == '_' || c6 == ' ')
                        {
                            value.score += 30;
                        }

                        int prev_mi = 0;
                        int num13 = -1;
                        for (int num14 = 0; num14 < fuzzyMatchData.matches_indx[m - 1].Count; num14++)
                        {
                            int i3 = fuzzyMatchData.matches_indx[m - 1][num14].i;
                            if (i3 >= value.i)
                            {
                                break;
                            }

                            int num15 = fuzzyMatchData.matches_indx[m - 1][num14].score;
                            if (i3 == value.i - 1)
                            {
                                num15 += 75;
                            }

                            if (num13 < num15)
                            {
                                num13 = num15;
                                prev_mi = num14;
                            }
                        }

                        value.score += num13;
                        value.prev_mi = prev_mi;
                        fuzzyMatchData.matches_indx[m][n] = value;
                    }
                }
            }

            int index2 = 0;
            int num16 = length - 1;
            for (int num17 = 1; num17 < fuzzyMatchData.matches_indx[num16].Count; num17++)
            {
                if (fuzzyMatchData.matches_indx[num16][index2].score < fuzzyMatchData.matches_indx[num16][num17].score)
                {
                    index2 = num17;
                }
            }

            ScoreIndx scoreIndx = fuzzyMatchData.matches_indx[num16][index2];
            outScore = scoreIndx.score;
            if (matches != null)
            {
                using (new ScopedProfiler("[FM] Matches calc"))
                {
                    matches.Capacity = length;
                    matches.Add(scoreIndx.i + i);
                    int prev_mi2 = scoreIndx.prev_mi;
                    for (int num18 = length - 2; num18 >= 0; num18--)
                    {
                        matches.Add(fuzzyMatchData.matches_indx[num18][prev_mi2].i + i);
                        prev_mi2 = fuzzyMatchData.matches_indx[num18][prev_mi2].prev_mi;
                    }

                    matches.Reverse();
                }
            }

            return true;
        }
    }
}