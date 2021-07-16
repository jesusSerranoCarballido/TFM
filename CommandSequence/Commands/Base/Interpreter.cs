using System.Data;

namespace CommandSequence.Commands.Base
{
    public class Interpreter :IInterpreter
    {
        DataTable dt;
        public Interpreter(DataTable data) {
            dt = data;
        }
        public DataTable Execute() {
            return dt;
        }

    }
}
