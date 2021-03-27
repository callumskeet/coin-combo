using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;

namespace CoinRepresentation
{
    /// <summary>Class <c>CoinRepresentation</c> provides a static solve method which
    ///     solves a variant of the subset-sum problem.</summary>
    public class CoinRepresentation
    {
        /// <summary>This method find the number of coin combinations that
        ///     add up to <paramref name="sum"/>. 
        ///     The set of coins is defined by: 
        ///     {k ∈ N | 2^k where k: 0 &lt;= k &lt; log_2(<paramref name="sum"/>)}
        ///     Where possible combinations can use a coin 0 to 2 times (inclusive)</summary>
        /// <param><c>sum</c> is the target value to find combinations for.</param>
        /// <returns>The number of possible combinations that sum to <paramref name="sum"/></returns>
        /// <see cref="CountCombinations"/>
        public static long Solve(long sum)
        {
            // The largest k such that 2^k <= sum
            long maxExponent = (long)(Math.Log(sum) / Math.Log(2));
            
            // Create ascending array of all 2^k exponentials less than sum
            // and calculate the total of all weights.
            long[] weights = new long[maxExponent + 1];
            long totalWeight = 0;
            for (var k = 0; k <= maxExponent; k++)
            {
                long w = (long)Math.Pow(2, k);
                weights[k] = w;
                totalWeight += w;
            }

            // There can be no subset in weights that equals sum if
            // two times (since each weight can be included twice) 
            // the sum of weights is less than sum. 
            if (2 * totalWeight < sum) 
            {
                return 0;
            }
            Hashtable table = new Hashtable();
            return CountCombinations(weights, sum, weights.Length - 1, totalWeight, table);
        }

        /// <summary>Finds the number of combinations in <paramref name="weights"/> that
        ///     sum to <paramref name="target"/>. Valid combinations can exclude, include
        ///     or twice include a value in <paramref name="weights"/>.
        /// <example>Example of initial call:
        /// <code>
        ///     long[] weights = new long[] { 1L, 2L, 3L, 4L, 5L, 6L };
        ///     long remaining = 0;
        ///     for (int i = 0; i &lt; weights.Length; i++) remaining += weights[i];
        ///     CountCombinations(weights, 42L, weights.Length - 1, remaining, new Hashtable());
        /// </code>
        /// </example>
        /// </summary>
        /// <param><c>weights</c> is the set of values to find combinations of.</param>
        /// <param><c>target</c> is the value that combinations are being searched for.</param>
        /// <param><c>i</c> corresponds to an index of <paramref name="weights"/>.</param>
        /// <param><c>remaining</c> is the combined sum of weights[0...i] inclusive.</param>
        /// <param><c>table</c> stores the results of completed function calls.</param>
        /// <returns>The number of valid combinations that summed to <paramref name="target"/>
        ///     in <paramref name="weights"/>.</returns>
        /// <remarks>This method uses recursion to solve this subset-sum problem. 
        ///     <para>Without optimisations, this method is O(<c>3^n</c>) where <c>n</c>
        ///     is the length of <paramref name="weights"/>. The total number of nodes in the
        ///     recursion tree is approximately <c>(3^(n+1))/2</c>. However, pruning and 
        ///     memoisation significantly reduce the number of recursive calls 
        ///     required.</para>
        ///     <para>The hashtable requires O(<c>2n</c>) memory. Considering use of a 
        ///     hashtable significantly reduced execution time the trade-off here
        ///     is highly favourable. Analysis indicated that duplicate states are a rare
        ///     occurrence. Therefore the time savings are a result of where duplicate
        ///     states occur, at high levels of the recursion stack. This allows the
        ///     algorithm to effectively prune a significant portion of calls.</para>
        ///     <para>I used this video lecture https://www.youtube.com/watch?v=kyLxTdsT8ws to
        ///     understand the problem. My algorithm is based of off the teachings given
        ///     there.</para>
        /// </remarks>
        /// <see cref="Solve"/>
        private static long CountCombinations(long[] weights, long target, long i, long remaining, Hashtable table)
        {
            // Find previously calculated result if it exists
            string key = $"{target}#{i}";
            if (table.Contains(key))
            {
                return (long)table[key];
            }

            // The target value was found
            if (target == 0)
            {
                return 1;
            }
            
            // Current state fails bounding conditions
            // 1. i has gone outside of the array
            // 2. No combination of the remaining weights will sum to the target
            if ((i < 0) || (2 * remaining < target))
            {
                return 0;
            }

            // Update the sum value of the remaining weights
            remaining -= weights[i];

            // Recursive operations
            // Loop steps:
            //  0. Exclude the current item from the subset
            //  1. Include the current item in the subset
            //  2. Include two of the same item in the subset
            // In each step check if the target will be overshot to reduce
            // recursion stack.
            long count = 0;
            for (int timesIncluded = 0; timesIncluded < 3; timesIncluded++)
            {
                long weightGained = timesIncluded * weights[i];
                if (target >= weightGained)
                {
                    count += CountCombinations(weights, target - weightGained, i - 1, remaining, table);
                }
            }

            // Add the current state to the table
            table.Add(key, count);
            return count;
        }
    }
}