using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshwaterQueueProcessing
{
    /// <summary>
    /// Generates a stream of randomly arriving customers.
    /// </summary>
    public class CustomerGenerator
    {
        private int[] customerArrivals;
        private readonly int minDuration;
        private readonly int maxDuration;
        private Random rand;

        /// <summary>
        /// Creates the generator and initializes the random number generator for populating the customer queues.
        /// </summary>
        /// <param name="minDuration">The minimum amount of time that it will take to process a customer's request.</param>
        /// <param name="maxDuration">The maximum amount of time that it will take to process a customer's request.</param>
        /// <param name="avgPerPeriod">The average number of customers that will arrive per time slot.</param>
        /// <param name="totalTime">Total number of time slots for which customers are to be generated.</param>
        /// <param name="seed">If present, the this value is passed as the seed to the random number generator.  If the seed is negative, no seed is passed.</param>
        public CustomerGenerator(int minDuration, int maxDuration, double avgPerPeriod, int totalTime, int seed = -1)
        {
            if (seed < 0)
                rand = new Random();
            else
                rand = new Random(seed);

            customerArrivals = new int[totalTime];
            this.minDuration = minDuration;
            this.maxDuration = maxDuration;
            initializeArrivals(avgPerPeriod, totalTime);
        }

        /// <summary>
        /// Initializes an array containing the number of customers that will be generated for each time slot.
        /// </summary>
        /// <param name="avgPerPeriod">The average number of customers that will arrive per time slot.</param>
        /// <param name="numTimePeriods">Total number of time slots for which customers are to be generated.</param>
        private void initializeArrivals(double avgPerPeriod, int numTimePeriods)
        {
            for (int i = 0; i < numTimePeriods * avgPerPeriod; i++)
            {
                int slot = rand.Next(numTimePeriods);
                customerArrivals[slot]++;
            }
        }

        /// <summary>
        /// Returns a Queue of customers that are generated for the given time slot.
        /// </summary>
        /// <param name="timeIndex">The time slot for which the customers are to be generated.</param>
        /// <returns>The Queue of customers.  This queue may be empty.</returns>
        public Queue<Customer> GetCustomers(int timeIndex)
        {
            Queue<Customer> customers = new Queue<Customer>();

            // Make cettain we haven't gone beyond the time; this could happen whlle
            // the queue of waiting customers is emptied
            if (timeIndex < customerArrivals.Length)
            {
                // Our pre-generated array contains the number of customers for any given time slot
                for (int numArrivals = 0; numArrivals < customerArrivals[timeIndex]; numArrivals++)
                {
                    int duration = rand.Next(maxDuration - minDuration + 1) + minDuration;
                    customers.Enqueue(new Customer(timeIndex, duration));
                }
            }
            return customers;
        }
    }
}

