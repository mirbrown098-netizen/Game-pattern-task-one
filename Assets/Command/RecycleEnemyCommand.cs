public class RecycleEnemyCommand : ICommand
{
    private FactoryTest factoryTest;

    public RecycleEnemyCommand(FactoryTest factoryTest)
    {
        this.factoryTest = factoryTest;
    }

    public void Execute()
    {
        factoryTest.ReturnLastSpawnedEnemy();
    }
}