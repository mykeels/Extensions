using System;
using System.Collections.Generic;
using System.Threading;

namespace Test_Extensions
{
    public class Promise
    {
        public Promise.State state { get; set; }
        public Thread current { get; set; }
        private Action<dynamic> success { get; set; }

        private List<Action<dynamic>> then = new List<Action<dynamic>>();
        private Action done { get; set; }
        private Action<Exception> error { get; set; }
        private Func<dynamic> work { get; set; }

        public Promise(Func<dynamic> func)
        {
            this.state = State.Pending;
            this.work = func;
            this.Execute();
        }

        public static Promise Create(Func<dynamic> func)
        {
            return new Promise(func);
        }

        private void Execute()
        {
            current = new Thread(() =>
            {
                try
                {
                    dynamic result = work();
                    if (success != null) {
                        success(result);
                        if (state.Equals(State.Pending)) state = State.Fulfilled;
                    }
                    if (then.Count > 0)
                    {
                        then.ForEach((action) =>
                        {
                            action(result);
                        });
                    }
                }
                catch (Exception ex)
                {
                    if (state.Equals(State.Pending)) state = State.Rejected;
                    if (error != null) error(ex);
                    Console.WriteLine(ex);
                }
                try
                {
                    if (done != null) done();
                }
                catch (Exception ex)
                {
                    if (state.Equals(State.Pending)) state = State.Rejected;
                    if (error != null) error(ex);
                    Console.WriteLine(ex);
                }
            });
            current.SetApartmentState(ApartmentState.STA);
            current.Start();
        }

        public Promise Success(Action<dynamic> act)
        {
            this.success = act;
            return this;
        }

        public Promise Then(Action<dynamic> act)
        {
            this.then.Add(act);
            return this;
        }

        public Promise Done(Action act)
        {
            this.done = act;
            return this;
        }

        public Promise Error(Action<Exception> act)
        {
            this.error = act;
            return this;
        }

        public enum State
        {
            Pending,
            Fulfilled,
            Rejected
        }
    }
}
