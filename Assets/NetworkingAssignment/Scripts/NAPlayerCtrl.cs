using UnityEngine;

using Photon.Pun;
using TMPro;


public class NAPlayerCtrl : MonoBehaviourPun
{
    public TextMeshProUGUI nickNameText; // �г��� ǥ�ÿ� TextMeshProUGUI

    void Start()
    {
        if (photonView.IsMine)
        {
            // ������ �г��� ����
            photonView.RPC("SetNickName", RpcTarget.AllBuffered, PhotonNetwork.NickName);
        }
    }

    [PunRPC]
    void SetNickName(string nickName)
    {
        // �г����� ��� Ŭ���̾�Ʈ���� ����ȭ
        nickNameText.text = nickName;
    }
}
