using System;

namespace EducationPortal.Domain.Core.Common
{
    public class ServiceResult
    {
        protected Exception exception;

        public ServiceResult()
        {
        }

        public ServiceResult(Exception exception)
        {
            this.exception = exception;
        }

        public Exception Exception => this.exception;

        public bool IsSuccessful => this.Exception == null;
    }

    public class ServiceResult<TValue> : ServiceResult
    {
        public ServiceResult()
            : base()
        {
        }

        public ServiceResult(Exception exception)
            : base(exception)
        {
        }

        public ServiceResult(TValue value)
        {
            this.Value = value;
        }

        public TValue Value { get; protected set; }
    }
}
