function TesteCors() {
    const tokenJWT = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImRlYWRzcGFjZUBob3RtYWlsLmNvbSIsInN1YiI6ImZhMDgzZGNlLWE2MDAtNDgxMy04NTFkLTcyYjZiZmEwZTEyMCIsImV4cCI6MTYyODc3OTkzMX0.rAmoOTA1PGZHVirZ7n2Ltsr0bqOhm1UbtGRIV_4RbD4";
    const servico = "https://localhost:44373/api/v1.0/mensagem/fa083dce-a600-4813-851d-72b6bfa0e120/96bdec9b-1cbb-449c-87bb-60d3f4077b18";

    $("#resultado").html("---Solicitando---");
    $.ajax({
        url: servico,
        method: "GET",
        crossDomain: true,
        headers: {"Accept": "application/json"},
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + tokenJWT);
        },
        success: function (data, status, xhr) {
            $("#resultado").html(data);
        }
    });
}
