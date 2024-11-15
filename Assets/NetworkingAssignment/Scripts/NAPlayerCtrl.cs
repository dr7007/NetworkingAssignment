using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class NAPlayerCtrl : MonoBehaviourPun
{
    [SerializeField]
    private string nickName = null;
    [SerializeField]
    private int curIdx = 0;

    private RectTransform rectTr = null;
    private TextMeshProUGUI playerUI = null;

    private void Awake()
    {
        rectTr = this.GetComponentInChildren<RectTransform>();
        playerUI = this.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        nickName = PhotonNetwork.NickName;
        playerUI.text = nickName;
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        rectTr.anchoredPosition = new Vector2(0f, -400f - curIdx * 100);

    }

    [PunRPC]
    public void ApplyIndex(int _idx)
    {
        curIdx = _idx;
        Debug.LogErrorFormat("{0} curIdx: {1}",
            PhotonNetwork.NickName,
            curIdx
            );
    }
}
