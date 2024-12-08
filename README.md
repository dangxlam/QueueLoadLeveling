# Queue-Based Load Leveling pattern
## 1. Giới thiệu về mẫu thiết kế Queue-based Load Leveling.
Bối cảnh hiện nay:
-	Dữ liệu và số lượng người dùng trong các hệ thống lớn ngày càng tăng, khi có quá nhiều yêu cầu cùng một lúc, có thể xảy ra tình trạng tắc nghẽn, dẫn đến giảm hiệu suất và thời gian phản hồi lâu.
-	Việc phân bổ tài nguyên không đồng đều dẫn đến tình trạng một số máy chủ thì quá tải trong khi một số khác thì không được sử dụng hết công suất.
-	Khi phải xử lý lượng truy cập lớn thì hệ thống khó mở rộng,  dẫn đến sự cố và ảnh hưởng đến trải nghiệm người dùng.

⮕ Cần có cơ chế để phân phối tải công việc đồng đều giữa các tài nguyên (máy chủ, dịch vụ) để tránh tình trạng tắc nghẽn và tối ưu hóa hiệu suất.
#### Giải pháp: 
Kiến trúc Queue-Based Load Leveling giải quyết những vấn đề này bằng cách sử dụng hàng đợi giữa tác vụ và dịch vụ để lưu trữ các yêu cầu và phân phối chúng đến các tài nguyên xử lý một cách hiệu quả. Từ đó giúp giảm thiểu tắc nghẽn, tăng cường khả năng mở rộng và tôi ưu hóa việc sử dụng tài nguyên.
Queue-based Load Leveling Pattern là một mẫu thiết kế nhằm quản lý lưu lượng công việc, tối ưu hóa việc xử lý và phân phối tải công việc trong các hệ thống phân tán bằng cách sử dụng hàng đợi. Thay vì xử lý yêu cầu ngay lập tức, hệ thống sẽ đưa các yêu cầu vào một hàng đợi và xử lý chúng theo một cơ chế nhất định, giúp giảm tải cho hệ thống và tránh tình trạng quá tải. Mẫu này giúp cân bằng tải giữa các thành phần của hệ thống, đảm bảo rằng không có thành phần nào bị quá tải trong khi các thành phần khác lại không được sử dụng.

![image](https://github.com/user-attachments/assets/6333a741-aa99-46d1-80e0-e605272027ea)
## 2. Giới thiệu các khái niệm quan trọng.
Khái niệm cơ bản:
-	Task: Đây là nhiệm vụ hoặc công việc cụ thể mà hệ thống cần thực hiện. Nó có thể là bất kỳ hoạt động nào cần xử lý, chẳng hạn như xử lý dữ liệu, gửi email, hoặc thực hiện tính toán.
-	Message: Đây là thông điệp chứa thông tin về nhiệm vụ cần thực hiện. Một message có thể bao gồm các chi tiết như loại công việc, dữ liệu cần xử lý, và các thông tin bổ sung khác để giúp hệ thống thực hiện nhiệm vụ một cách chính xác.
-	Queue: Đây là hàng đợi nơi các message được lưu trữ trước khi chúng được xử lý. Queue giúp phân tán tải công việc bằng cách cho phép hệ thống tiếp nhận và lưu trữ các nhiệm vụ mà không cần phải xử lý ngay lập tức. Các worker (người xử lý) sẽ lấy message từ queue để thực hiện các task theo cách không đồng bộ.
-	Worker: Đây là một thành phần hoặc một đơn vị thực thi chịu trách nhiệm xử lý các công việc (tasks) được đưa vào hàng đợi (queue).
## 3. Các thành phần, luồng hoạt động cơ bản.
#### Các thành phần chính trong mẫu kiến trúc này:
-	Producers (Máy phát sinh yêu cầu): Producers là các thực thể tạo ra các tác vụ hoặc yêu cầu cần được hệ thống xử lý. Đây có thể là người dùng, ứng dụng hoặc hệ thống bên ngoài. Họ gửi các tác vụ đến hàng đợi thay vì trực tiếp đến thành phần xử lý.
-	Hàng đợi (Bộ đệm): Hàng đợi đóng vai trò là trung gian giữa producer và consumer. Hàng đợi lưu trữ tạm thời các yêu cầu đến, cho phép xử lý chúng ở tốc độ được kiểm soát. Hàng đợi này rất quan trọng trong việc tách biệt tốc độ tạo ra các yêu cầu khỏi tốc độ xử lý chúng, ngăn ngừa tình trạng quá tải hệ thống trong thời gian lưu lượng truy cập tăng đột biến.
-	Consumers (Bộ xử lý yêu cầu): Consumers là các dịch vụ hoặc máy chủ chịu trách nhiệm xử lý các tác vụ từ hàng đợi. Họ lấy và xử lý các yêu cầu với tốc độ phù hợp với tài nguyên khả dụng của họ, đảm bảo rằng họ không bị quá tải. Consumers có thể mở rộng quy mô động dựa trên nhu cầu khối lượng công việc, thêm hoặc xóa tài nguyên khi cần thiết.
-	Load Balancer: Trong một số triển khai, bộ cân bằng tải được sử dụng để phân phối khối lượng công việc đều cho nhiều người dùng. Điều này đảm bảo rằng không có người dùng nào bị quá tải và tài nguyên được sử dụng tối ưu. Bộ cân bằng tải cũng có thể giúp cải thiện khả năng chịu lỗi bằng cách định tuyến lại các tác vụ nếu người dùng bị lỗi.
-	Công cụ giám sát và kiểm soát: Các công cụ giám sát rất quan trọng để quan sát độ dài hàng đợi, thời gian xử lý và hiệu suất tổng thể của hệ thống. Thành phần này giúp xác định các điểm nghẽn, ngăn hàng đợi phát triển quá lớn (có thể gây ra sự chậm trễ) và đảm bảo người dùng hoạt động hiệu quả. Một số hệ thống sử dụng vòng phản hồi để điều tiết hoặc ưu tiên các tác vụ dựa trên kích thước hàng đợi hoặc tình trạng hệ thống.

#### Cách thức hoạt động: 
 Khi một yêu cầu được gửi đến hệ thống, thay vì xử lý ngay lập tức, yêu cầu này sẽ được đưa vào hàng đợi. Sau đó, một hay nhiều worker sẽ lấy yêu cầu từ hàng đợi và xử lý chúng. Quá trình này giúp điều chỉnh tải, đảm bảo rằng hệ thống không bị quá tải khi có nhiều yêu cầu đến cùng lúc. Cụ thể:
- Bước 1:
Các producer (như người dùng, ứng dụng hoặc dịch vụ khác, ví dụ như một web server nhận yêu cầu từ người dùng) tạo ra các công việc và gửi yêu cầu hoặc tác vụ đến hệ thống. Các công việc có thể là xử lý dữ liệu, lưu trữ vào cơ sở dữ liệu hoặc các tác vụ bất đồng bộ khác.
- Bước 2:
Các tác vụ được giữ trong hàng đợi theo thứ tự cho đến khi chúng được chuẩn bị để xử lý, hàng đợi này đóng vai trò như một bộ đệm cho phép hệ thống xử lý các tác vụ ở tốc độ ổn định, được điều chỉnh thay vì quá tải với quá nhiều tác vụ cùng một lúc. Các công việc này sẽ được giữ an toàn và không bị mất cho đến khi được xử lý
- Bước 3:
Các nhiệm vụ có mức độ ưu tiên cao có thể được xử lý sớm hơn, trong khi các nhiệm vụ cơ mức độ ưu tiên thấp hơn phải chờ lâu hơn. Điều này dựa trên các yếu tố như tính cấp bách, tầm quan trọng hoặc trạng thái của khách hàng.
- Bước 4:
Consumer (máy chủ hoặc thành phần xử lý) lấy tác vụ từ hàng đợi và xử lý chúng. Tốc độ xử lý được xác định bởi nguồn lực sẵn có và khả năng của consumer.
- Bước 5:
Hệ thống kiểm tra thời gian xử lý, độ dài hàng đợi và tình trạng chung của hệ thống. Dựa vào đó, hệ thống có thể mở rộng quy mô bằng cách thêm nhiều consumer hơn hoặc hạn chế số lượng producer để giảm số lượng yêu cầu nếu hàng đợi bắt đầu tăng quá mức, một dấu hiệu cho thấy nhu cầu vượt quá khả năng xử lý. Mặt khác, hệ thống có thể thu hẹp quy mô để tiết kiệm tài nguyên nếu hàng đợi gần như trống rỗng.
## 4. Ưu và nhược điểm.
#### Ưu điểm:
-	Tăng cường khả năng chịu lỗi: Nếu một worker gặp sự cố, công việc có thể được chuyển đến các worker khác mà không làm gián đoạn toàn bộ hệ thống, giúp cải thiện tính khả dụng.
-	Mở rộng linh hoạt: Dễ dàng thêm nhiều worker để xử lý công việc khi khối lượng công việc tăng mà không cần thay đổi cấu trúc của hệ thống.
-	Tối ưu hóa tài nguyên: Hệ thống có thể điều chỉnh số lượng worker dựa trên tải thực tế.
#### Nhược điểm:
-	Độ trễ: Hàng đợi có thể gây ra độ trễ trong việc xử lý yêu cầu, đặc biệt là khi hàng đợi quá dài.
-	Có một số rủi ro trong việc quản lý hàng đợi như sự cố mất thông tin do giới hạn hệ thống.
## 5. Các công cụ hỗ trợ
Hiện nay có nhiều công cụ và thư viện giúp triển khai Queue-Based Load Leveling trong các ứng dụng, ví dụ:
-	RabbitMQ: Là một công cụ phổ biến để quản lý message queue, hỗ trợ mô hình Queue-Based Load Leveling rất hiệu quả.
-	Apache Kafka: Một hệ thống phân tán mạnh mẽ, chủ yếu dùng để xử lý luồng dữ liệu lớn.
-	Amazon SQS: Dịch vụ hàng đợi tin nhắn của Amazon Web Services (AWS) giúp bạn dễ dàng triển khai mô hình này trong môi trường đám mây.
-	Azure Storage Queue: Là dịch vụ lưu trữ số lượng lớn tin nhắn. Bạn truy cập tin nhắn từ mọi nơi trên thế giới thông qua các cuộc gọi được xác thực bằng HTTP hoặc HTTPS.
## 6. Một số ứng dụng trong thực tế
#### Một số lĩnh vực được ứng dụng:
-	Thương mại điện tử: Quản lý các đơn đặt hàng trong các mùa mua sắm cao điểm.
-	Dịch vụ web: Xử lý các yêu cầu từ người dùng trong các ứng dụng web lớn. Ví dụ các công việc xử lý video (như chuyển mã video, tạo thumbnail, v.v.) có thể được đưa vào queue và các worker sẽ xử lý chúng theo từng công đoạn.
-	Xử lý dữ liệu: Quản lý và xử lý khối lượng lớn dữ liệu trong các hệ thống phân tích. Ví dụ các email marketing có thể được gửi vào queue và các consumer sẽ xử lý và gửi email cho khách hàng.
#### Ví dụ cụ thể:
-	Amazon Web Service – SQS: đây là một ví dụ điển hình về việc cân bằng tải dựa trên hàng đợi trong thực tế.  Nó là dịch vụ message queue được quản lý hoàn toàn cho phép các hệ thống phân tán tách rời các thành phần, đảm bảo rằng các tác vụ được lưu trữ trong hàng đợi cho đến khi chúng có thể được xử lý. Các dịch vụ như AWS Lambda hoặc EC2 có thể kéo message từ SQS khi chúng đã sẵn sàng, xử lý các tác vụ ở tốc độ được kiểm soát, điều này giúp ngăn ngừa tình trạng quá tải hệ thống khi lưu lượng truy cập tăng đột biến. lý tưởng cho các dịch vụ có nhu cầu cao như xử lý đơn hàng thương mại điện tử hoặc đăng ký người dùng.
-	Netflix: Netflix sử dụng Queue-based load leveling pattern rộng rãi để quản lý khối lượng công việc mã hóa và chuyển mã video. Khi nội dung mới được tải lên hoặc cập nhật, nội dung đó phải được xử lý ở nhiều định dạng và độ phân giải khác nhau để phù hợp với các thiết bị khác nhau (TV, điện thoại, máy tính bảng,…). Hàng đợi chứa các tác vụ chuyển mã này và đảm bảo rằng các máy chủ khả dụng (máy chủ khách) xử lý chúng ở tốc độ không làm quá tải hệ thống. Các tiếp cận này cũng đảm bảo rằng các tài nguyên mã hóa có thể mở rộng linh hoạt theo nhu cầu giúp toàn bộ quy trình trở nên hiệu quả.
-	Uber: Uber sử dụng hệ thống cân bằng tải theo hàng đợi để xử lý các yêu cầu đặt xe vào những thời điểm có nhu cầu cao. Khi hàng ngàn khách hàng yêu cầu đặt xe cùng một lúc, hệ thống sẽ xếp hàng các yêu cầu này và kết nối họ với những tài xế đã sẵn sàng. Điều này tách biệt nhu cầu đặt xe khỏi nguồn cung cấp tài xế hiện có, đảm bảo hệ thống có thể mở rộng quy mô và phản ứng mà không bị quá tải.
## Slide
Truy cập slide tại: https://www.canva.com/design/DAGKa6KAzLY/4dbuzApNHVYDPtaCN7PE0g/edit
## Cài đặt

1. Clone

```bash
git clone https://github.com/dangxlam/QueueLoadLeveling.git
```
2. Set up
 ```bash
- Consumer/local.settings.json: điền azureservicebus accesskey vào "ServiceBusConnectionString": ""
- Producer/config.json: điền azureservicebus accesskey vào "ServiceBusConnectionString": ""
```

3. Send Message

```bash
cd Producer
dotnet run
```
4. Receive Message
```bash
cd Consumer
func start
```
## Tài liệu tham khảo
[1] https://learn.microsoft.com/en-us/azure/architecture/patterns/queue-based-load-leveling

[2] https://www.geeksforgeeks.org/queue-based-load-leveling-pattern-system-design/

## Contributing
Welcome
