using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshwaterQueueProcessing
{
    public class CustomerQueue
    {
        public Queue<Customer> tellerQ;
        public int transDuration = 0;

        public CustomerQueue()
        {
            tellerQ = new Queue<Customer>();
        }

       public void enqueueCustomer(Customer c)
        {
            transDuration += c.TransactionDuration;
        }

        public void dequeueCustomer(Customer c)
        {
            transDuration -= c.TransactionDuration;
        }
    }
}
