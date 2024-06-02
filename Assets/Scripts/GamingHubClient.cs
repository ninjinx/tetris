using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using MagicOnion.Client;
using Shared.Interfaces;
using UnityEngine;

public class GamingHubClient : IGamingHubReceiver
{
    private List<Player> _players = new List<Player>();

    private IGamingHub _client;

    private GameManager _gameManager;

    private string _ownPlayerName;

    // コンストラクタ
    public GamingHubClient(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    // methods send to server.
    public async Task JoinAsync(ChannelBase grpcChannel, string roomName, string playerName)
    {
        _client = await StreamingHubClient.ConnectAsync<IGamingHub, IGamingHubReceiver>(grpcChannel, this);

        var roomPlayers = await _client.JoinAsync(roomName, playerName);

        Debug.Log("--- Joined Players ---");
        foreach (var player in roomPlayers)
        {
            Debug.Log("name: " + player.Name);
        };
        Debug.Log("----------------------");

        _ownPlayerName = playerName;
    }

    public async Task OjamaAsync()
    {
        await _client.OjamaAsync();
    }

    // dispose client-connection before channel.ShutDownAsync is important!
    public Task DisposeAsync()
    {
        return _client.DisposeAsync();
    }

    // You can watch connection state, use this for retry etc.
    public Task WaitForDisconnect()
    {
        return _client.WaitForDisconnect();
    }

    // Receivers of message from server.

    void IGamingHubReceiver.OnJoin(Player player)
    {
        Debug.Log("Received Join Player:" + player.Name);

        // 自分以外の場合は配列に追加
        if (_ownPlayerName != player.Name)
        {
            _players.Add(player);
        }
    }

    void IGamingHubReceiver.OnLeave(Player player)
    {
        Debug.Log("Leave Player:" + player.Name);
    }

    void IGamingHubReceiver.OnOjama(Player player)
    {
        Debug.Log("Ojama Pushed:" + player.Name);

        // 自分以外のユーザーからOjamaを受け取った場合はログを出力
        if (_ownPlayerName != player.Name)
        {
            Debug.Log("Ojama!");

            // おじゃまブロックを生成
            _gameManager.GenerateOjamaBlock();
        }
    }
}