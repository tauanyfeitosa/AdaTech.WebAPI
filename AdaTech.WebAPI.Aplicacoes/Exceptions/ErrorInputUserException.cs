﻿namespace AdaTech.WebAPI.Aplicacoes.Exceptions
{
    public class ErrorInputUserException: Exception
    {
        public ErrorInputUserException() : base() { }
               
        public ErrorInputUserException(string message) : base(message) { }
               
        public ErrorInputUserException(string message, Exception inner) : base(message, inner) { }
    }
}
