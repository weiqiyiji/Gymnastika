namespace Gymnastika.Data.Migration.Commands
{
    public class DropIndexCommand : TableCommand
    {
        public string IndexName { get; set; }

        public DropIndexCommand(string tableName, string indexName)
            : base(tableName)
        {
            IndexName = indexName;
        }
    }
}
