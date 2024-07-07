import { Component, inject } from '@angular/core';
import { HttpService } from '../../http.service';
import {MatTableModule} from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { RouterLink } from '@angular/router';
import { IRestaurant } from '../../interfaces/restaurant';
import {MatIconModule} from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ConfirmationDialogComponent } from '../../confirmation-dialog/confirmation-dialog.component';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-restaurant-list',
  standalone: true,
  imports: [MatTableModule,MatButtonModule,RouterLink,MatIconModule,],
  templateUrl: './restaurant-list.component.html',
  styleUrl: './restaurant-list.component.scss'
})
export class RestaurantListComponent {
  router=inject(Router);
  restaurantList:IRestaurant[]=[];
  httpService=inject(HttpService);
  toastr=inject(ToastrService);
  displayedColumns: string[] = ['name', 'location', 'cuisineType','contactNumber','email','operatingHours','website','action'];
  constructor(public dialog: MatDialog){}
  ngOnInit(): void {
    this.httpService.getAllRestaurantList().subscribe(
      (result: any[]) => {
        this.restaurantList = result;
        console.log(this.restaurantList);
      },
      (error) => {
        console.error('Error fetching restaurants:', error);
        // Handle error as needed (e.g., show error message)
      }
    );
  }
  openConfirmationDialog(id:any){
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width: '300px',
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // Perform delete operation or other actions
        this.httpService.DeleteRestaurant(id).subscribe(()=>{
          console.log("deleted");
          this.restaurantList=this.restaurantList.filter(x=>x.restroId!=id);
          this.toastr.success("Deleted");
         
        });
      } else {
        console.log('User cancelled deletion');
      }
    });
  }
  edit(id:any){
    console.log(id);
    this.router.navigateByUrl("/restaurant/"+id);
  }
  delete(id:any){
    this.openConfirmationDialog(id);
  }
}
