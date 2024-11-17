using UnityEngine;

using Photon.Pun;
using TMPro;


public class NAPlayerCtrl : MonoBehaviourPun
{
    public TextMeshProUGUI nickNameText; // 닉네임 표시용 TextMeshProUGUI

    void Start()
    {
        if (photonView.IsMine)
        {
            // 본인의 닉네임 설정
            photonView.RPC("SetNickName", RpcTarget.AllBuffered, PhotonNetwork.NickName);
        }
    }

    [PunRPC]
    void SetNickName(string nickName)
    {
        // 닉네임을 모든 클라이언트에서 동기화
        nickNameText.text = nickName;
    }
}
