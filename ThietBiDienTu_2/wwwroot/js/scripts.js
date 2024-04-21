<script>
    function GetTrangThaiString(trangThai) {
        if (trangThai == 0) {
            return "Chưa duyệt";
        }
        else if (trangThai == 1) {
            return "Chưa trả";
        }
        else if (trangThai == 2){
            return "Đã duyệt"
        }
        else if (trangThai == 3){
            return "Đã trả"
        }
        else if (trangThai == 4){
            return "Từ chối"
        }
        else if (trangThai == 5){
            return "Hủy phiếu"
        }
        else if (trangThai == 6){
            return "Không mượn"
        }
    return ""; // Trạng thái không xác định
    }

    
</script>

