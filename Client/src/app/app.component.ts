import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  title = 'Dating APp';
users :any;
  constructor(private httpClient: HttpClient){
  }

  ngOnInit(): void {
    this.getUserList();
  }

  getUserList()
  {
    this.httpClient.get('https://localhost:5001/api/users').subscribe(data => {
      this.users = data;
    },
    (error=>{
      console.log(error)
    })
    );
  }
}
