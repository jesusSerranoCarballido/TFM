using System.Data;

namespace CommandSequence.Commands.Base
{
    public interface IInterpreter
    {
        DataTable Execute();
    }
}
