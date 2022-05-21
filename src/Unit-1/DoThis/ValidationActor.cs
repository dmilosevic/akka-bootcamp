using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace WinTail
{
    public class ValidationActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;

        public ValidationActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            var msg = message as string;
            if (string.IsNullOrEmpty(msg))
            {
                _consoleWriterActor.Tell(new Messages.NullInputError("No input received"));
            }
            else
            {
                var valid = IsValid(msg);
                if (valid)
                {
                    _consoleWriterActor.Tell(new Messages.InputSuccess("Thank you! Message was valid."));
                }
                else
                {
                    _consoleWriterActor.Tell(new Messages.ValidationError("Invalid: input has odd number of characters"));
                }
            }
            Sender.Tell(new Messages.ContinueProcessing());
        }

        /// <summary>
        /// Determines if the message received is valid.
        /// Checks if number of chars in message received is even.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool IsValid(string msg)
        {
            return msg.Length % 2 == 0;
        }
    }
}
