using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshwaterQueueProcessing
{
    
    /// <summary>
    /// Keeps track of the arrival time, the time the service of the customer begins and the expected duration of the transaction, i.e. how long it will take to perform the customer's request.
    /// </summary>
    public class Customer
    {
        // The time (i.e. minutes from the start of the simulation) that the customer arrives
        public readonly int ArrivalTime;

        // The number of minutes that it will take to service this customer
        public readonly int TransactionDuration;
        
        // The constructor is called from the generator; you never need to create a customer
        public Customer(int arrival, int duration)
        {
            ArrivalTime = arrival;
            TransactionDuration = duration;
        }

        // For debugging purposes
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Customer [arrivalTime=");
            builder.Append(ArrivalTime);
            builder.Append(", transactionDuration=");
            builder.Append(TransactionDuration);
            builder.Append("]");
            return builder.ToString();
        }
    }

}
