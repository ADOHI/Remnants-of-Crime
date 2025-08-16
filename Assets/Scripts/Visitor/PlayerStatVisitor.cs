public interface IStatVisitor
{
    void Visit(PlayerStatsData stats);
}

public class IncreaseHPVisitor : IStatVisitor
{
    private int amount;
    public IncreaseHPVisitor(int amount) { this.amount = amount; }
    public void Visit(PlayerStatsData stats) { stats.hp += amount; }
}

public class IncreaseStaminaVisitor : IStatVisitor
{
    private int amount;
    public IncreaseStaminaVisitor(int amount) { this.amount = amount; }
    public void Visit(PlayerStatsData stats) { stats.stamina += amount; }
}

public class IncreaseCashGainVisitor : IStatVisitor
{
    private float amount;
    public IncreaseCashGainVisitor(float amount) { this.amount = amount; }
    public void Visit(PlayerStatsData stats) { stats.cashGainRate += amount; }
}

public class IncreaseAttackVisitor : IStatVisitor
{
    private int amount;
    public IncreaseAttackVisitor(int amount) { this.amount = amount; }
    public void Visit(PlayerStatsData stats) { stats.attack += amount; }
}
