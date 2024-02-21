﻿namespace AdaTech.WebAPI.SistemaVendas.Utilities.Exceptions
{
    public class FailCreateUpdateException: Exception
    {
        public FailCreateUpdateException() : base() { }

        public FailCreateUpdateException(string message) : base(message) { }

        public FailCreateUpdateException(string message, Exception inner) : base(message, inner) { }
    }
}
