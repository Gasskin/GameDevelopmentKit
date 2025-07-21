using System.Net;
using Cysharp.Threading.Tasks;
using GameFramework;
using GameFramework.Event;
using GameFramework.Fsm;
using Luban;
using UnityEngine;
using UnityGameFramework.Extension;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<Game.Hot.ProcedureComponent>;

namespace Game.Hot
{
    public sealed class ProcedureLaunch : ProcedureBase
    {
        private int? _joinRoomForm;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            _joinRoomForm = GameEntry.UI.OpenUIForm(UIFormId.JoinRoomForm);
            HotEntry.Model.Room.ChangeBattleStage(EBattleStage.None);
            
            HotEntry.Tables.LoadAsync(LoadByteBuf).Forget();
        }

        protected override void OnUpdate(IFsm<ProcedureComponent> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (HotEntry.Model.Room.BattleStage == EBattleStage.InRoom)
                ChangeState<ProcedureInRoom>(procedureOwner);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            if (_joinRoomForm is > 0)
                GameEntry.UI.CloseUIForm((int)_joinRoomForm);
        }
        
        async UniTask<ByteBuf> LoadByteBuf(string file)
        {
            TextAsset textAsset = await GameEntry.Resource.LoadAssetAsync<TextAsset>(AssetUtility.GetLubanHotAsset(file, false));
            return new ByteBuf(textAsset.bytes);
        }
    }
}