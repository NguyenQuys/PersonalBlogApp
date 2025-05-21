@model List<Person>

<!-- DataTables CSS -->
<link rel = "stylesheet" href= "https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css" />

< table id= "personTable" class= "display" >
    < thead >
        < tr >
            < th > Id </ th >
            < th > Tên </ th >
            < th > Tuổi </ th >
        </ tr >
    </ thead >
    < tbody >
        @foreach(var person in Model)
        {
            < tr >
                < td > @person.Id </ td >
                < td > @person.Name </ td >
                < td > @person.Age </ td >
            </ tr >
        }
    </ tbody >
</ table >

< !--jQuery và DataTables JS -->
@section Scripts {
    <script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>
    <script>
        $(document).ready(function () {
            $('#personTable').DataTable();
        });
    </ script >
}

