import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './Artists.css'; // Import the CSS file for styling

const Artists = () => {
    const [artists, setArtists] = useState([]);
    const [selectedArtist, setSelectedArtist] = useState(null);
    const [name, setName] = useState('');
    const [birthDate, setBirthDate] = useState('');
    const [famousWork, setFamousWork] = useState('');
    const [bio, setBio] = useState('');

    useEffect(() => {
        fetchArtists();
    }, []);

    const fetchArtists = () => {
        axios.get('https://localhost:7166/api/artists')
            .then(response => {
                setArtists(response.data);
            })
            .catch(error => {
                console.error('There was an error fetching the artists!', error);
            });
    };

    const fetchArtistById = (id) => {
        axios.get(`https://localhost:7166/api/artists/${id}`)
            .then(response => {
                setSelectedArtist(response.data);
                setName(response.data.name);
                setBirthDate(response.data.birthDate);
                setFamousWork(response.data.famousWork);
                setBio(response.data.bio);
            })
            .catch(error => {
                console.error('There was an error fetching the artist by ID!', error);
            });
    };

    const addArtist = () => {
        const newArtist = { name, birthDate, famousWork, bio };
        axios.post('https://localhost:7166/api/artists', newArtist)
            .then(response => {
                fetchArtists();
                setSelectedArtist(response.data);
                setName('');
                setBirthDate('');
                setFamousWork('');
                setBio('');
            })
            .catch(error => {
                console.error('There was an error adding the artist!', error);
            });
    };

    const updateArtist = () => {
        if (!selectedArtist) return;

        const updatedArtist = { ...selectedArtist, name, birthDate, famousWork, bio };
        axios.put(`https://localhost:7166/api/artists/${selectedArtist.artistId}`, updatedArtist)
            .then(() => {
                fetchArtists();
                setSelectedArtist(null);
                setName('');
                setBirthDate('');
                setFamousWork('');
                setBio('');
            })
            .catch(error => {
                console.error('There was an error updating the artist!', error);
            });
    };

    const deleteArtist = (id) => {
        axios.delete(`https://localhost:7166/api/artists/${id}`)
            .then(() => {
                fetchArtists();
            })
            .catch(error => {
                console.error('There was an error deleting the artist!', error);
            });
    };

    const resetForm = () => {
        setSelectedArtist(null);
        setName('');
        setBirthDate('');
        setFamousWork('');
        setBio('');
    };

    return (
        <div className="artists-container">
            <h2>Artists</h2>
            <ul>
                {artists.map(artist => (
                    <li key={artist.artistId}>
                        {artist.name}
                        <button onClick={() => fetchArtistById(artist.artistId)}>Edit</button>
                        <button onClick={() => deleteArtist(artist.artistId)}>Delete</button>
                    </li>
                ))}
            </ul>
            <div className="edit-form">
                <h3>{selectedArtist ? 'Edit Artist' : 'Add Artist'}</h3>
                <label>Name:</label>
                <input type="text" value={name} onChange={(e) => setName(e.target.value)} />
                <label>Birth Date:</label>
                <input type="date" value={birthDate} onChange={(e) => setBirthDate(e.target.value)} />
                <label>Famous Work:</label>
                <input type="text" value={famousWork} onChange={(e) => setFamousWork(e.target.value)} />
                <label>Bio:</label>
                <textarea value={bio} onChange={(e) => setBio(e.target.value)}></textarea>
                <button onClick={selectedArtist ? updateArtist : addArtist}>
                    {selectedArtist ? 'Update' : 'Add'}
                </button>
                {selectedArtist && <button onClick={resetForm}>Cancel</button>}
            </div>
            {selectedArtist && (
                <div className="artist-details">
                    <h3>Artist Details</h3>
                    <p><strong>Name:</strong> {selectedArtist.name}</p>
                    <p><strong>Birth Date:</strong> {selectedArtist.birthDate}</p>
                    <p><strong>Famous Work:</strong> {selectedArtist.famousWork}</p>
                    <p><strong>Bio:</strong> {selectedArtist.bio}</p>
                </div>
            )}
        </div>
    );
};

export default Artists;
