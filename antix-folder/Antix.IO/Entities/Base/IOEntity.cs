namespace Antix.IO.Entities.Base
{
    public abstract class IOEntity
    {
        readonly string _identifier;

        public string Identifier
        {
            get { return _identifier; }
        }

        protected IOEntity(string identifier)
        {
            _identifier = identifier;
        }
    }
}