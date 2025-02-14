using System;
using System.Net;

namespace Lookif.Library.Common.Exceptions;
public class NotReadyException : AppException
{ 
    public NotReadyException()
        : base(ApiResultStatusCode.SuccessButNotReady)
    {
        base.HttpStatusCode = HttpStatusCode.OK;
    }

    public NotReadyException(string message)
        : base(ApiResultStatusCode.SuccessButNotReady, message)
    {
        base.HttpStatusCode = HttpStatusCode.OK;
    }
 
}
