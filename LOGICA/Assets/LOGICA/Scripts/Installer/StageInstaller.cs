using LOGICA.Model;
using Zenject;

namespace LOGICA.Installer
{
    public class StageInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameStateModel>().AsSingle();
        }
    }
}