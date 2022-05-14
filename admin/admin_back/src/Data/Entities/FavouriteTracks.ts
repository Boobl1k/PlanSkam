import {Entity, Column, PrimaryGeneratedColumn} from 'typeorm';

@Entity('FavouriteTracks')
export class FavouriteTracks {
    @PrimaryGeneratedColumn()
    Id: number;
}