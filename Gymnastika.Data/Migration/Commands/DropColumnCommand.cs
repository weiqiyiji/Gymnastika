namespace Gymnastika.Data.Migration.Commands
{
    public class DropColumnCommand : ColumnCommand
    {

        public DropColumnCommand(string tableName, string columnName)
            : base(tableName, columnName)
        {
        }
    }
}
