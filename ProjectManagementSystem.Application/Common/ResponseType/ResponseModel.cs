namespace GamePlanBackend.Application.Common.ResponseType
{
    
        public partial class ResponseModel
        {
            public string ResponseMessage { get; set; } = string.Empty; 
            public bool IsError { get; set; }
            public dynamic DataModel { get; set; } = null; 
            public int statusCode { get; set; }
            public bool Success { get; set; }
            public string ErrorDetails { get; set; } = string.Empty;
            public object Error { get; set; } = null;
        }
    

}
