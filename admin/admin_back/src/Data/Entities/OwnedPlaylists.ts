import {Entity, Column, PrimaryGeneratedColumn} from 'typeorm';

@Entity('OwnedPlaylists')
export class OwnedPlaylists {
    @PrimaryGeneratedColumn()
    Id: number;
}