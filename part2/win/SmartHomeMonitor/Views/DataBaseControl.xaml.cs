using MahApps.Metro.Controls;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using SmartHomeMonitoringApp.Logics;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartHomeMonitoringApp.Views
{
    /// <summary>
    /// DataBaseControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DataBaseControl : UserControl
    {
        // 초기에 필요한 변수 or 속성 선언
        public Boolean IsConnected {  get; set; }
        Thread MqttThread { get; set; } // 없으면 UI컨트롤과 충돌나서 Log를 찍지 못함(응답없음)
        int MaxCount { get; set; } // MQTT로그 과적으로 속도저하 방지
        public DataBaseControl()
        {
            InitializeComponent();
        }

        // 유저컨트롤 화면 로드된 이후 초기화
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TxtBrokerUrl.Text = Commons.BROKERHOST;
            TxtMqttTopic.Text = Commons.MQTTTOPIC;
            TxtConnString.Text = Commons.DBCONNSTRING;

            IsConnected = false;
            BtnConnect.IsChecked = false;
        }

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            ConnectSystem(); // DB연결, MQTT 연결
        }

        private async void ConnectSystem()
        {
            if(IsConnected == false) // 연결이 안됐을 때 처리
            {
                // 한번도 접속을 안했으면 모두 연결
                var mqttFactory = new MqttFactory();
                Commons.MQTT_CLIENT = mqttFactory.CreateMqttClient();
                // MQTT 브로커 아이피 연결속성
                var mqttClientQptions = new MqttClientOptionsBuilder().WithTcpServer(TxtBrokerUrl.Text).Build();

                await Commons.MQTT_CLIENT.ConnectAsync(mqttClientQptions, CancellationToken.None); // MQTT연결
                Commons.MQTT_CLIENT.ApplicationMessageReceivedAsync += MQTT_CLIENT_ApplicationMessageReceivedAsync;

                var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder().WithTopicFilter(
                    f =>
                    {
                        f.WithTopic(Commons.MQTTTOPIC);
                    }).Build();

                await Commons.MQTT_CLIENT.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
                
                IsConnected = true;
                BtnConnect.IsChecked = true;
                BtnConnect.Content = "MQTT 연결중";
            }
            else
            {
                // 연결된 뒤에 연결끊기
                if(Commons.MQTT_CLIENT.IsConnected)
                {
                    Commons.MQTT_CLIENT.ApplicationMessageReceivedAsync -= MQTT_CLIENT_ApplicationMessageReceivedAsync; // 이벤트와 연결을 끊을 때는 -= 사용
                    await Commons.MQTT_CLIENT.DisconnectAsync();

                    IsConnected = false;
                    BtnConnect.IsChecked = false;
                    BtnConnect.Content = "Connect";
                }
            }
        }

        private Task MQTT_CLIENT_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            var payload = Encoding.UTF8.GetString(arg.ApplicationMessage.Payload);
            //Debug.WriteLine(payload);
            UpdateLog(payload); // Thread 처리를 해주는 메서드
            InsertData(payload);

            return Task.CompletedTask;
        }

        private void InsertData(string payload)
        {
            this.Invoke(() =>
            {
                var currValue = JsonConvert.DeserializeObject<Dictionary<string, string>>(payload); // Dictionary 형태로 바뀌어서 넘어옴
                //Debug.WriteLine("InsertData :" + currValue["CURR_DT"]);
                if (currValue != null)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(TxtConnString.Text))
                        {
                            conn.Open();
                            var insQuery = @"INSERT INTO [dbo].[SmartHomeData]
                                                       ( [DEV_ID]
                                                       , [CURR_DT]
                                                       , [TEMP]
                                                       , [HUMID])
                                                 VALUES
                                                       ( @DEV_ID
                                                       , @CURR_DT
                                                       , @TEMP
                                                       , @HUMID)";

                            SqlCommand cmd = new SqlCommand(insQuery, conn);
                            cmd.Parameters.AddWithValue("@DEV_ID", currValue["DEV_ID"]);
                            cmd.Parameters.AddWithValue("@CURR_DT", currValue["CURR_DT"]); // string --> DateTime 자동 형변환
                            var splitValue = currValue["VALUE"].Split('|'); // splitValue[0] -> 온도, splitValue[1] -> 습도
                            cmd.Parameters.AddWithValue("@TEMP", splitValue[0]);
                            cmd.Parameters.AddWithValue("@HUMID", splitValue[1]);

                            if(cmd.ExecuteNonQuery() == 1)
                            {
                                UpdateLog(">>>> DB Insert succeed");
                            }
                            else
                            {
                                UpdateLog(">>>> DB Insert failed.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        UpdateLog($"DB 에러발생 : {ex.Message}");
                    }
                }
            });
            
        }

        private void UpdateLog(string payload)
        {
            this.Invoke(() =>
            {
                TxtLog.Text += $"{payload}\n";
                TxtLog.ScrollToEnd(); // 스크롤이 생기기 시작하면 제일 아래로 포커스
            });
        }
    }
}
