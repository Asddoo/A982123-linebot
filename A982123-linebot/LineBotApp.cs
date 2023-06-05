using Line.Messaging;
using Line.Messaging.Webhooks;

namespace A982123_linebot;

public class LineBotApp : WebhookApplication
{
    private readonly LineMessagingClient _messagingClient;

    private static Dictionary<string, string> _pool = new Dictionary<string, string>();

    public LineBotApp(LineMessagingClient lineMessagingClient)
    {
        _messagingClient = lineMessagingClient;
    }

    protected override async Task OnMessageAsync(MessageEvent ev)
    {
        var result = null as List<ISendMessage>;

        switch (ev.Message)
        {
            //文字訊息
            case TextEventMessage textMessage:
            {
                //頻道Id
                var channelId = ev.Source.Id;
                //使用者Id
                var userId = ev.Source.UserId;
                //使用者輸入的文字
                var text = ((TextEventMessage)ev.Message).Text;

                if (PoolHasMsg(text))
                {
                    // 從記憶體池查詢資料
                    string response = GetResponse(text);
                    result = new List<ISendMessage>
                    {
                        new TextMessage(response)
                    };
                }
                else
                {
                    if (CheckFormat(text))
                    {
                        //將資料寫入記憶體池
                        TeachDog(text);
                    }
                }
                /*
                //回傳 hellow
                result = new List<ISendMessage>
                {
                    new TextMessage("小雞雞")
                };
                */
            }
                break;
        }

        if (result != null)
            await _messagingClient.ReplyMessageAsync(ev.ReplyToken, result);
    }

    private bool PoolHasMsg(string InputMsg)
    {
        return _pool.ContainsKey(InputMsg);
    }

    private string GetResponse(string inputMsg)
    {
        return _pool[inputMsg];
    }
    
    private bool CheckFormat(string inputMsg)
    {
        bool result = false;

        try
        {
            string[] subs = inputMsg.Split(';');

            if (subs.Length == 3)
            {
                if (subs[0] == "ABC")
                {
                    result = true;
                }
            }
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            throw;
        }
        
        return result;
    }

    private void TeachDog(string inputMsg)
    {
        
    }
}