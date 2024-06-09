Database ở mức vừa đủ cho môn học  không mở rộng nhưng cũng không bị sai quá nhiều.
### GV góp ý:
+ Lý do từ chối và lý do hủy trong phiếu mượn có thể gom thành 1 lý do khác tại vì 2 cái đó không đồng thời tồn tại
+ Tổng chi phí trong phiếu sửa có thể bỏ đi vì nó không phải chức năng dùng nhiều có thể tiết kiệm bộ nhớ thay cho việc load data nhanh
  
![image](https://github.com/ltmichael52/Quan_Ly_Thiet_Bi/assets/101556054/ec07b2ac-9299-4d0b-b1f3-a0eb6a0c3b17)

Trong phòng có 1 field đặc biệt là độ ưu tiên. <br>Vì sinh viên thì chỉ cho hiện dòng thiết bị mà không cho hiện chọn cụ thể thiết bị nào nên sẽ để phần mềm tự động chọn thiết bị cụ thể
- Khi sinh viên mượn --> phần mềm tự lấy trong kho có độ ưu tiên cao hơn --> lấy theo thứ tự giảm dần của seri trong kho đó (này là giả sử nhà trường họ xếp thiết bị theo seri giảm dần nền phần mềm sẽ lấy theo giảm dần)   
