import {Entity, Column, PrimaryGeneratedColumn} from 'typeorm';

//название таблицы не соответствует

@Entity()
export class OwnedPlaylists {
    @PrimaryGeneratedColumn()
    Id: number;
}