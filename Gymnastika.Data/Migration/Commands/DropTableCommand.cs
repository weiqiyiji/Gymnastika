namespace Gymnastika.Data.Migration.Commands
{
    public class DropTableCommand : SchemaCommand
    {
        public DropTableCommand(string name)
            : base(name, SchemaCommandType.DropTable)
        {
        }
    }
}
