// Em /Models/Shared/OperationResult.cs
namespace ChessaSystem.Models.Shared
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public static OperationResult SuccessResult() => new() { Success = true };
        public static OperationResult FailResult(string message) => new() { Success = false, Message = message };
    }
}