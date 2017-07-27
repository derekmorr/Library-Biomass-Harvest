using Edu.Wisc.Forest.Flel.Util;
using Landis.Library.BiomassCohorts;
using Landis.Library.SiteHarvest;
using System.Collections.Generic;

namespace Landis.Library.BiomassHarvest
{
    /// <summary>
    /// Selects specific ages and ranges of ages among a species' cohorts
    /// for harvesting.
    /// </summary>
    public class SpecificAgesCohortSelector
    {
        private static Percentage defaultPercentage;

        private AgesAndRanges agesAndRanges;
        private IDictionary<ushort, Percentage> percentages;

        //---------------------------------------------------------------------

        static SpecificAgesCohortSelector()
        {
            defaultPercentage = Percentage.Parse("100%");
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="ages">List of individual ages that are selected.</param>
        /// <param name="ranges">List of age ranges that are selected.</param>
        /// <param name="percentages">The percentages for each age or range.
        /// The percentage for a range is indexed by the starting age of the
        /// range.
        /// </param>
        public SpecificAgesCohortSelector(IList<ushort>                   ages,
                                          IList<AgeRange>                 ranges,
                                          IDictionary<ushort, Percentage> percentages)
        {
            agesAndRanges = new AgesAndRanges(ages, ranges);
            this.percentages = new Dictionary<ushort, Percentage>(percentages);
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Selects which cohorts are harvested.
        /// </summary>
        /// <returns>
        /// true if the given cohort is to be harvested.  The cohort's biomass
        /// should be reduced by the percentage returned in the second
        /// parameter.
        /// </returns>
        public bool Selects(ICohort cohort, out Percentage percentage)
        {
                ushort ageToLookUp = 0;
                AgeRange? containingRange;
                if (agesAndRanges.Contains(cohort.Age, out containingRange))
                {
                    if (! containingRange.HasValue)
                        ageToLookUp = cohort.Age;
                    else {
                        ageToLookUp = containingRange.Value.Start;
                    }
                    if (! percentages.TryGetValue(ageToLookUp, out percentage))
                        percentage = defaultPercentage;
                    return true;
                }
                percentage = null;
                return false;
        }
    }
}
