public class SpawnEnemyCommand : ICommand
{
    private FactoryTest factoryTest;
    private int enemyType;

    public SpawnEnemyCommand(FactoryTest factoryTest, int enemyType)
    {
        this.factoryTest = factoryTest;
        this.enemyType = enemyType;
    }

    public void Execute()
    {
        if (enemyType == 0)
        {
            factoryTest.SpawnWalker();
        }
        else if (enemyType == 1)
        {
            factoryTest.SpawnRunner();
        }
    }
}