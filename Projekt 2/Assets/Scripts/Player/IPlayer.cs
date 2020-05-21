
public interface IPlayer
{
    float Hp { get; set; }
    float MaxHp { get; set; }
    float Speed { get; set; }

    void GetDmg(float dmg);
    void GameOver();
}
