using System.Collections.Generic;

public class PlayScreenUpdate(List<IEnemyGroup> enemies, RedEnemy redEnemy) {

    public void EnemiesUpdate() {
        foreach (IEnemyGroup enemy in enemies)
        {
            enemy.Update();
        }

        redEnemy?.Update();
    }
}