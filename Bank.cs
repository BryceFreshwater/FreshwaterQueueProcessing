using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshwaterQueueProcessing
{
    class Bank
    {
        public static void Main(string[] args)
        {
            // shortest time to service a customer; must be greater than zero
            int MINIMUM_DURATION = 1;

            // longest time to service a customer; must be greater than the minimum duration
            int MAXIMUM_DURATION = 5;

            // average customers arriving per given minute.  This would mean three customers every four minutes.
            double CUST_PER_MINUTE = 5;

            // how long the simulation represents; 120 would equal two hours
            int SIMULATION_LENGTH = 120;

            // The number of tellers for this run of the simulation
            int NUM_TELLERS = 10;

            // Create the customer generator
            CustomerGenerator frontdoor = new CustomerGenerator(MINIMUM_DURATION, MAXIMUM_DURATION, CUST_PER_MINUTE, SIMULATION_LENGTH);
            CustomerQueue customerLine = new CustomerQueue();
            List<Teller> tellers = new List<Teller>();
            List<CustomerQueue> tellerLines = new List<CustomerQueue>();

            Console.WriteLine("(O)ne queue");
            Console.WriteLine("(S)hortest queue");
            Console.WriteLine("(F)astest queue");
            char choice = char.Parse(Console.ReadLine());
            // Print the parameters, i.e. the constants the program began with
            Console.WriteLine("Customer Generator Parameters:\n*******************");
            Console.WriteLine("Minimum Customer Service Time: {0} Minutes", MINIMUM_DURATION);
            Console.WriteLine("Maximum Customer Service Time: {0} Minutes", MAXIMUM_DURATION);
            Console.WriteLine("Average Arriving Customers per Minute: {0}", CUST_PER_MINUTE);
            Console.WriteLine("Simulation Length: {0} Minutes", SIMULATION_LENGTH);
            Console.WriteLine("Number of Tellers: {0}\n", NUM_TELLERS);

            switch (choice)
            {
                case 'O':
                case 'o':
                    for (int i = 0; i < NUM_TELLERS; i++)
                    {
                        Teller teller = new Teller(customerLine);
                        tellers.Add(teller);
                    }
                    break;
                case 'S':
                case 's':
                case 'F':
                case 'f':
                    for (int i = 0;i < NUM_TELLERS; i++)
                    {
                        tellerLines.Add(new CustomerQueue());
                        Teller teller = new Teller(tellerLines[i]);
                        tellers.Add(teller);
                    }
                    break;
            }

            int maxWaitTime = 0;
            int maxIdleTime = 0;
            int totalCustomers = 0;
            double avgIdleTime = 0;
            double avgWaitTime = 0;
            int totalIdleTime = 0;
            int totalWaitTime = 0;
            int currentTime = 0;
            int shortestLine = 0;
            int fastestLine = 0;
            

            while (currentTime < SIMULATION_LENGTH) 
            {
                // get the queue of arriving customers from the frontdoor
                Queue<Customer> arrivals = frontdoor.GetCustomers(currentTime);
                switch (choice)
                {
                    case 'O':
                    case 'o':

                        for (int i = 0; i < arrivals.Count; i++)
                        {
                            customerLine.tellerQ.Enqueue(arrivals.Dequeue());
                        }
                        break;

                    case 'S':
                    case 's':
                        
                        for (int i = 0; i < arrivals.Count; i++)
                        {
                            int shortestIdx = 0;
                            for (int j = 0; j < tellerLines.Count; j++)
                            {
                                if (tellerLines[j].tellerQ.Count <= shortestLine) 
                                {
                                    shortestIdx = j;
                                }
                            }
                            tellerLines[shortestIdx].tellerQ.Enqueue(arrivals.Dequeue());
                        }
                        break;

                    case 'F':
                    case 'f':
                        for (int i = 0;i < arrivals.Count;i++)
                        {
                            int fastestIdx = 0;
                            for (int j = 0;j < tellerLines.Count;j++)
                            {
                                if (tellerLines[j].transDuration <= fastestLine)
                                {
                                    fastestIdx = j;
                                }
                            }
                            Customer customer = arrivals.Dequeue();
                            tellerLines[fastestIdx].tellerQ.Enqueue(customer);
                            tellerLines[fastestIdx].enqueueCustomer(customer);                           
                        }
                        break;
                }
                foreach (Teller teller in tellers)
                {
                    teller.ProcessNextCustomer(currentTime);
                }
                currentTime++;
            }
            foreach (Teller teller in tellers)
            {
                if (teller.IdleTime > maxIdleTime) { maxIdleTime = teller.IdleTime; }
                if (teller.MaxWaitTime > maxWaitTime) { maxWaitTime = teller.MaxWaitTime; }
                totalCustomers += teller.CustomersServed;
                totalIdleTime += teller.IdleTime;
                totalWaitTime += teller.TotalWaitTime;
            }

            avgIdleTime = Convert.ToDouble(totalIdleTime) / Convert.ToDouble(totalCustomers);
            avgWaitTime = Convert.ToDouble(totalWaitTime) / Convert.ToDouble(totalCustomers);
            Console.WriteLine("Statistics:\n*******************");
            Console.WriteLine("Maximum Teller Idle Time: {0} Minutes", maxIdleTime);
            Console.WriteLine("Average Teller Idle Time: {0:0.00} Minutes", avgIdleTime);
            Console.WriteLine("Maximum Customer Wait Time: {0} Minutes", maxWaitTime);
            Console.WriteLine("Average Customer Wait Time: {0:0.00} Minutes\n", avgWaitTime);
            Console.WriteLine("Number of Customers Serviced: {0}", totalCustomers);
            Console.WriteLine("Total Time Taken: {0} Minutes", currentTime);
            Console.ReadLine();
        }
    }
}
