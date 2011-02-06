using Gymnastika.Data.Migration.Commands;

namespace Gymnastika.Data.Migration.Interpreters {
    public interface IDataMigrationInterpreter {
        void Visit(ISchemaBuilderCommand command);
        void Visit(CreateTableCommand command);
        void Visit(DropTableCommand command);
        void Visit(AlterTableCommand command);
        void Visit(SqlStatementCommand command);
        void Visit(CreateForeignKeyCommand command);
        void Visit(DropForeignKeyCommand command);
    }
}
