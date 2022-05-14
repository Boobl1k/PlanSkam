import {Entity, Column, PrimaryGeneratedColumn} from 'typeorm';

//название таблицы не соответствует

@Entity()
export class FavouriteTracks {
    @PrimaryGeneratedColumn()
    Id: number;
}