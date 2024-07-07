import { Routes } from '@angular/router';
import { RestaurantListComponent } from './admin/restaurant-list/restaurant-list.component';
import { RestaurantFormComponent } from './admin/restaurant-form/restaurant-form.component';

export const routes: Routes = [
    {
        path:"",
        component:RestaurantListComponent
    },
    {
        path:"restaurant-list",
        component:RestaurantListComponent
    },
    {
        path:"create-restaurant",
        component:RestaurantFormComponent
    },
    {
        path:"restaurant/:id",
        component:RestaurantFormComponent
    }
];
