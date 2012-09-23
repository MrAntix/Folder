namespace Antix.IO.Entities.Base
{
    public abstract class IOEntity
    {
        readonly string _identifier;

        public string Identifier
        {
            get { return _identifier; }
        }

        protected IOEntity(Parameters parameters)
        {
            _identifier = parameters.Identifier;
        }

        public class Parameters
        {
            public string Identifier { get; set; }
        }
    }
}