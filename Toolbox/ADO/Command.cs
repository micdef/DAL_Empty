using System;
using System.Collections.Generic;

namespace Toolbox.ADO
{
    public class Command
    {
        private IDictionary<string, object> _parameters;
        private bool _isStoredProcedure;
        private string _query;

        public Command(string query, bool isStoredProcedure = false)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Query is not valid");
            this.Query = query;
            this.IsStoredProcedure = isStoredProcedure;
            this._parameters = new Dictionary<string, object>();
        }

        internal IDictionary<string, object> Parameters
        {
            get { return _parameters; }
            private set { _parameters = value; }
        }

        internal bool IsStoredProcedure
        {
            get { return _isStoredProcedure; }
            private set { _isStoredProcedure = value; }
        }

        internal string Query
        {
            get { return _query; }
            private set { _query = value; }
        }

        public void AddParameter(string parameterName, object value)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentException("Name is not valid");
            if (value is null)
                throw new NullReferenceException("Value is not valid");
            _parameters.Add(parameterName, value ?? DBNull.Value);
        }

        public void RemoveParameter(string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentException("The parameter name cannot be null");
            if (!_parameters.ContainsKey(parameterName))
                throw new MissingMemberException($"The parameter {parameterName} is not a member of the parameters list");
            _parameters.Remove(parameterName);
        }

        public void ClearParameters()
        {
            _parameters.Clear();
        }
    }
}
