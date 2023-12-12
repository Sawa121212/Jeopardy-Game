namespace Common.Core.Interfaces
{
    public interface IResult<out TResult>
    {
        TResult GetResult();
    }
}