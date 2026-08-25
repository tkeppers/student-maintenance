using System;

namespace DojoStudentManagement
{
    /// <summary>
    /// One rung of the rank ladder, from the Ranks table: WHITE (order 1) through HACHIDAN
    /// (order 13). RankNext is "NA" at the top of the ladder.
    /// </summary>
    public class Rank
    {
        /// <summary>Sentinel stored in Ranks.rank_next for the top of the ladder.</summary>
        public const string NoNextRank = "NA";

        public string RankID { get; set; }
        public int RankOrder { get; set; }
        public string RankNext { get; set; }

        public bool HasNextRank =>
            !string.IsNullOrWhiteSpace(RankNext) &&
            !string.Equals(RankNext.Trim(), NoNextRank, StringComparison.OrdinalIgnoreCase);
    }
}
