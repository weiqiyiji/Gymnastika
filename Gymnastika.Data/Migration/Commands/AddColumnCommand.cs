namespace Gymnastika.Data.Migration.Commands
{
    public class AddColumnCommand : CreateColumnCommand
    {
        public AddColumnCommand(string tableName, string name)
            : base(tableName, name)
        {
        }
    }
}
